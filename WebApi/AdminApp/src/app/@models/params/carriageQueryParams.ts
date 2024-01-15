import {QueryParams} from './queryParams';

export interface CarriageQueryParams extends QueryParams {
  trainId: number;
  carriageTypeId: number;
}
