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
  seatTypeId: number
  booked: boolean
  selected: boolean
}
