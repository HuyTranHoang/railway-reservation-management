import { Component } from '@angular/core';
import {MENU_ITEMS} from '../pages/pages-menu';
import {NbMenuItem} from '@nebular/theme';

@Component({
  selector: 'ngx-managements',
  template: `
    <ngx-one-column-layout>
      <nb-menu [items]="menu"></nb-menu>
      <router-outlet></router-outlet>
    </ngx-one-column-layout>`,
})

export class ManagementsComponent {
  menu: NbMenuItem[] = [
    {
      title: 'MANAGEMENTS',
      group: true,
    },
    {
      title: 'Railway',
      icon: 'home-outline',
      link: '/managements/railway',
    },
  ];
}
