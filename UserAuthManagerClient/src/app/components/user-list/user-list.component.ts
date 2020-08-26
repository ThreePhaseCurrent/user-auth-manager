import { Component, OnInit } from '@angular/core';
import {ShortUser} from '../../models/shortUser';
import {AuthService} from '../../services/auth.service';

import * as AOS from 'aos';
import {takeUntil} from 'rxjs/operators';
import {DestroyService} from '../../services/destroy.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  userList: ShortUser[];

  constructor(
    private authService: AuthService,
    private destroy$: DestroyService
  ) { }

  ngOnInit(): void {
    this.authService.getUsers()
      .pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.userList = x;
    });

    AOS.init();
  }

}
