import {Component, OnInit} from '@angular/core';
import {ClimateDataService} from "../../../services/climateDataService";

@Component({
  selector: 'app-climate-data',
  templateUrl: 'data.html'
})
export class ClimateDataComponent implements OnInit {
  public climateData = [];

  constructor(private _climateDataService: ClimateDataService) {
  }

  private loadData() {
    this._climateDataService.getData()
      .subscribe(data => {
        this.climateData = data;
      });
  }

  public ngOnInit(): void {
    this.loadData();
  }
}
