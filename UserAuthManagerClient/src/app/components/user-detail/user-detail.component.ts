import {Component, Input, OnInit} from '@angular/core';
import {ShortUser} from '../../models/shortUser';

import * as AOS from 'aos';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  @Input() user: ShortUser;

  constructor() { }

  ngOnInit(): void {
    AOS.init();
  }

}
