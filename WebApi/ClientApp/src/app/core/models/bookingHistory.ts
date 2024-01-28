export interface BookingHistory {
  upcomingTrips: UpcomingTrip[]
  pastTrips: PastTrips[]
  cancellations: Cancellations[]
}

export interface UpcomingTrip {
  id : string
  code: string
  passengerName: string
  trainName: string
  carriageName: string
  seatName: any
  scheduleName: string
  price: number
  status: string
  createdAt: string
}

export interface PastTrips {
  id : string
  code: string
  passengerName: string
  trainName: string
  carriageName: string
  seatName: any
  scheduleName: string
  price: number
  status: string
  createdAt: string
}

export interface Cancellations {
  id : string
  code: string
  passengerName: string
  trainName: string
  carriageName: string
  seatName: any
  scheduleName: string
  price: number
  status: string
  createdAt: string
}
