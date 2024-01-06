import { Component } from '@angular/core';
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
      title: 'Dashboard',
      icon: 'layers-outline',
      link: '/managements/dashboard',
      home: true,
    },
    {
      title: 'MANAGEMENTS',
      group: true,
    },
    {
      title: 'Railway',
      icon: 'backspace-outline',
      children: [
        {
          title: 'Train Company',
          link: '/managements/railway/train-company',
        },
        {
          title: 'Train Station',
          link: '/managements/railway/train-station',
        },
      ],
    },
    {
      title: 'Train & Carriage',
      icon: 'car-outline',
      children: [
        {
          title: 'Train',
          link: '/managements/train-and-carriage/train',
        },
        {
          title: 'Carriage',
          link: '/managements/train-and-carriage/carriage',
        },
        {
          title: 'Carriage Type',
          link: '/managements/train-and-carriage/carriage-type',
        },
        {
          title: 'Compartment',
          link: '/managements/train-and-carriage/compartment',
        },
      ],
    },
    {
      title: 'Seat and Seat Type',
      icon: 'pantone-outline',
      children: [
        {
          title: 'Seat',
          link: '/managements/seat-and-seat-type/seat',
        },
        {
          title: 'Train Station',
          link: '/managements/seat-and-seat-type/seat-type',
        },
      ],
    },
    {
      title: 'Schedule and Ticket Prices',
      icon: 'map-outline',
      children: [
        {
          title: 'Schedule',
          link: '/managements/schedule-and-ticket-prices/schedule',
        },
        {
          title: 'Distance Fare',
          link: '/managements/schedule-and-ticket-prices/distance-fare',
        },
        {
          title: 'Round Trip',
          link: '/managements/schedule-and-ticket-prices/round-trip',
        },
      ],
    },
    {
      title: 'Passenger and Ticket',
      icon: 'people-outline',
      children: [
        {
          title: 'Passenger',
          link: '/managements/passenger-and-ticket/passenger',
        },
        {
          title: 'Ticket',
          link: '/managements/passenger-and-ticket/ticket',
        },
      ],
    },
  ];
}
