export interface TrainDetail {
  id: number
  name: string
  trainCompany: TrainCompany
  carriages: Carriage[]
}

export interface TrainCompany {
  id: number
  name: string
  logo: string
}

export interface Carriage {
  id: number
  name: string
  type: Type
  compartments: Compartment[]
}

export interface Type {
  id: number
  name: string
}

export interface Compartment {
  id: number
  name: string
  seats: Seat[]
}

export interface Seat {
  id: number
  name: string
  carriageId: number
  compartmentId: number
  seatTypeId: number
  seatTypeName: string
  serviceCharge: number
  seatTotalPrice: number
  booked: boolean
  selected: boolean
}

export interface Passenger {
  title: string
  fullName: string
  passportNumber: string
  phoneNumber: string
  email: string
  dob: string
}
