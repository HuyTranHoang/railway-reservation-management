import {QueryParams} from './queryParams';

export interface ScheduleQueryParams extends QueryParams {
  trainId: number;
  departureStationId: number;
  arrivalStationId: number;
}
