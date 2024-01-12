import {Component, OnInit} from '@angular/core';
import {RoundTripService} from './round-trip.service';
import {RoundTrip} from '../../../@models/roundTrip';
import {Pagination} from '../../../@models/pagination';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {QueryParams} from '../../../@models/params/queryParams';
import {ShowRoundTripComponent} from './show-round-trip/show-round-trip.component';
import {ConfirmDeleteRoundTripComponent} from './confirm-delete-round-trip/confirm-delete-round-trip.component';
import {NbDialogService} from '@nebular/theme';
import {PaginationService} from '../../shared/pagination.service';

@Component({
  selector: 'ngx-round-trip',
  template: `
    <router-outlet></router-outlet>
  `,
})
export class RoundTripComponent {
}
