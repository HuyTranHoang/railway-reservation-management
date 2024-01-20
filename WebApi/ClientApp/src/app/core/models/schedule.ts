export interface Schedule {
  id: number
  name: string
  trainId: number
  trainName: string
  departureStationId: number
  departureStationName: string
  arrivalStationId: number
  arrivalStationName: string
  departureTime: string
  arrivalTime: string
  duration: number
  price: number
  status: string
  createdAt: string
}
