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
  seatName: string
  scheduleId: number
  scheduleName: string
  price: number
  isCancel: boolean
  status: string
  createdAt: string
}
