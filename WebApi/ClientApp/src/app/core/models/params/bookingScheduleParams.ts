export interface BookingScheduleParams {
  departureStationId: number;
  arrivalStationId: number;
  departureTime: string;
  returnDate: string | null;
  roundTrip: boolean;
}
