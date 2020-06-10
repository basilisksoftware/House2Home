import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-stats-box',
  templateUrl: './stats-box.component.html',
  styleUrls: ['./stats-box.component.css']
})
export class StatsBoxComponent implements OnInit {

  @Input() statTitle: string;
  @Input() stat: number;
  @Input() bgc: string;

  constructor() { }

  ngOnInit(): void {
  }

}
