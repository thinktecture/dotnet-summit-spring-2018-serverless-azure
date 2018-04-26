using HomeAutomation.Common;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio;

namespace HomeAutomationFunctions.Functions.Orchestrations
{
    public static class PhoneVerification
    {
        [FunctionName(MethodNames.SmsPhoneVerification)]
        public static async Task<bool> Run(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            TraceWriter log)
        {
            const int retries = 3;
            const int timeout = 90;

            var phoneNumber = context.GetInput<string>();

            if (String.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentNullException(
                    nameof(phoneNumber),
                    "A phone number is required.");
            }

            var challengeCode = await context.CallActivityAsync<int>(
                "SendSmsChallenge",
                phoneNumber);

            using (var timeoutCts = new CancellationTokenSource())
            {
                var expiration = context.CurrentUtcDateTime.AddSeconds(timeout);
                var timeoutTask = context.CreateTimer(expiration, timeoutCts.Token);

                var authorized = false;

                for (var retryCount = 0; retryCount <= retries; retryCount++)
                {
                    var challengeResponseTask =
                        context.WaitForExternalEvent<int>("SmsChallengeResponse");

                    var winner = await Task.WhenAny(challengeResponseTask, timeoutTask);

                    if (winner == challengeResponseTask)
                    {
                        if (challengeResponseTask.Result != challengeCode)
                        {
                            continue;
                        }

                        log.Info("### Phone number AUTHORIZED!");

                        // TODO: add phone number to 'user profile'
                        authorized = true;
                    }

                    break;
                }

                if (!timeoutTask.IsCompleted)
                {
                    timeoutCts.Cancel();
                }

                return authorized;
            }
        }

        [FunctionName(MethodNames.SendSmsChallenge)]
        public static int SendSmsChallenge(
            [ActivityTrigger]
            string phoneNumber,
            TraceWriter log,
            [TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "%TwilioFromNumber%")]
            out SMSMessage message)
        {
            var rand = new Random(Guid.NewGuid().GetHashCode());
            var challengeCode = rand.Next(10000);

            log.Info($"Sending verification code {challengeCode} to {phoneNumber}.");

            message = new SMSMessage
            {
                To = phoneNumber,
                Body = $"[MyHome] Your verification code for is {challengeCode:0000}"
            };

            return challengeCode;
        }
    }
}