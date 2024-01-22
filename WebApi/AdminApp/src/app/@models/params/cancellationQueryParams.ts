import {QueryParams} from './queryParams';

export interface CancellationQueryParams extends QueryParams {
  ticketId: number;
  cancellationRuleId: number;
}
