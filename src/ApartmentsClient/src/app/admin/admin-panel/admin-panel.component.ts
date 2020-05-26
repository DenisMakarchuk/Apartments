import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.scss']
})
export class AdminPanelComponent implements OnInit {

  clicker1 = false;
  clicker2 = false;

  constructor() { }

  ngOnInit(): void {
  }

  changeClicker1(){
    if (this.clicker1) {
      this.clicker1 = false;
    }
    else
    {
      this.clicker1 = true;
    }
  }

  changeClicker2(){
    if (this.clicker2) {
      this.clicker2 = false;
    }
    else
    {
      this.clicker2 = true;
    }
  }
}
