import {QueryParams} from './queryParams';

export interface TicketQueryParams extends QueryParams {
     passengerId: number;
     trainId: number;
     distanceFareId: number;
     carriageId: number;
     seatId: number;
     scheduleId: number;
     paymentId: number;
     createdAt: string;
}
