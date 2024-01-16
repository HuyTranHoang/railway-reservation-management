export interface Schedule {
  id: number;
  name: string;
  trainId: number;
  trainName: string;
  departureStationId: number;
  departureStationName: string;
  arrivalStationId: number;
  arrivalStationName: string;
  departureDate: string;
  arrivalDate: string;
  departureTime: string;
  duration: number;
  status: string;
  createdAt: string;
}
