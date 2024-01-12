import {Component, OnInit} from '@angular/core';
import {CarriageTypeService} from './carriage-type.service';
import {CarriageType} from '../../../@models/carriageType';
import {Pagination} from '../../../@models/pagination';
import {PaginatedResult} from '../../../@models/paginatedResult';
import {QueryParams} from '../../../@models/params/queryParams';
import {NbDialogService} from '@nebular/theme';
import {ShowCarriageTypeComponent} from './show-carriage-type/show-carriage-type.component';
import {
  ConfirmDeleteCarriageTypeComponent,
} from './confirm-delete-carriage-type/confirm-delete-carriage-type.component';

@Component({
  selector: 'ngx-carriage-type',
  template: `
    <router-outlet></router-outlet>
  `,
})
export class CarriageTypeComponent {

}
