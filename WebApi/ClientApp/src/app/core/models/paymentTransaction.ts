export interface PaymentTransaction {
  passengers: PaymentPassenger[]
  tickets: PaymentTicket[]
  paymentId: number
  trainId: number
  scheduleId: number
}

export interface PaymentPassenger {
  fullName: string
  cardId: string
  age: number
  gender: string
  phone: string
  email: string
}

export interface PaymentTicket {
  carriageId: number
  seatId: number
}

