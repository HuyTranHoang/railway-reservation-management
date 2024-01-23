export interface Cancellation {
    id: number;
    ticketId: number;
    ticketCode: string;
    cancellationRuleId: number;
    departureDateDifference: number;
    reason: string;
    status: string;
    createdAt: string;
}