import {animate, group, query, style, transition, trigger} from '@angular/animations';

export const routeTransition = trigger('routeTransition', [
  transition('starwars => home, pokemon => home, mirror => home', [
    query(':enter, :leave', style({position: 'fixed'}), {optional: true}),
    group([
      query(':enter', [
        style({transform: 'translate3d(-100%, 0, 0)', opacity: 0}),
        animate('.5s ease-in-out', style({transform: 'translate3d(0, 0, 0)', opacity: 1}))
      ], {optional: true}),
      query(':leave', [
        style({transform: 'translate3d(0, 0, 0)', opacity: 1}),
        animate('.5s ease-in-out', style({transform: 'translate3d(100%, 0, 0)', opacity: 0}))
      ], {optional: true})
    ])
  ]),
  transition('* <=> *', [
    query(':enter, :leave', style({position: 'fixed'}), {optional: true}),
    group([
      query(':enter', [
        style({transform: 'translate3d(100%, 0, 0)', opacity: 0}),
        animate('.5s ease-in-out', style({transform: 'translate3d(0, 0, 0)', opacity: 1}))
      ], {optional: true}),
      query(':leave', [
        style({transform: 'translate3d(0, 0, 0)', opacity: 1}),
        animate('.5s ease-in-out', style({transform: 'translate3d(-100%, 0, 0)', opacity: 0}))
      ], {optional: true})
    ])
  ]),
]);
