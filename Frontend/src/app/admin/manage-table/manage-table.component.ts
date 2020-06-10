import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-manage-table',
  templateUrl: './manage-table.component.html',
  styleUrls: ['./manage-table.component.css']
})
export class ManageTableComponent implements OnInit {

  @Input() donations: any;
  @Input() heading: string;
  @Input() isArchive: boolean;
  @Input() isSchedule: boolean;
  @Input() isManage: boolean;
  @Input() isReject: boolean;

  constructor() { }

  ngOnInit(): void {
  }

}
