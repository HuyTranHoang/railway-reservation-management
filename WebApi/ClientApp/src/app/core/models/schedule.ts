export interface ScheduleWithBookingParams {
  schedule: Schedule[]
  bookingParams: string
}

export interface Schedule {
  id: number
  name: string
  trainId: number
  trainName: string
  trainCompanyName: string
  trainCompanyLogo: string
  departureStationId: number
  departureStationName: string
  departureStationAddress: string
  arrivalStationId: number
  arrivalStationName: string
  arrivalStationAddress: string
  departureTime: string
  arrivalTime: string
  duration: number
  price: number
  status: string
  createdAt: string
  scheduleCarriageTypes: ScheduleCarriageType[]
  selectedCarriageType: ScheduleCarriageType
}

export interface ScheduleCarriageType {
  id: number
  name: string
  serviceCharge: number
}
