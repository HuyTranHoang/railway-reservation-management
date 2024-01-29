export interface BookingHistory {
  upcomingTrips: Ticket[]
  pastTrips: Ticket[]
  cancellations: Ticket[]
}

export interface Ticket {
  id: string
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
