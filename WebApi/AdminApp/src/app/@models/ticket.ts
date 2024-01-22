export interface Ticket {
    id: number;
    code: string;
    passengerId: number;
    passengerName: string;
    trainId: number;
    trainName: string;
    carriageId: number;
    carriageName: string;
    distanceFareId: number;
    seatId: number;
    seatName: string;
    scheduleId: number;
    scheduleName: string;
    paymentId: number;
    srice: number;
    status: string;
    createdAt : string;
}
