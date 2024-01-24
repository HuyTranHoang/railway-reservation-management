export interface Cancellation {
    id: number;
    ticketId: number;
    ticketCode: string;
    cancellationRuleId: number;
    cancellationRuleDepartureDateDifference: number;
    cancellationRuleFee: number;
    reason: string;
    status: string;
    createdAt: string;
}