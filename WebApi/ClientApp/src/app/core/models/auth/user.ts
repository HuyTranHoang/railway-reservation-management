export interface User {
  id : string
  firstName: string
  lastName: string
  email: string
  phoneNumber: any
  roles: string[]
  provider: string
  jwt: string
}
