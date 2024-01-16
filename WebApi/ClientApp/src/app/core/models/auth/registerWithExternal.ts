export class RegisterWithExternal {
  firstName: string
  lastName: string
  email: string
  userId: string
  accessToken: string
  provider: string

  constructor(firstName: string, lastName: string, email: string, userId: string, accessToken: string, provider: string) {
    this.firstName = firstName
    this.lastName = lastName
    this.email = email
    this.userId = userId
    this.accessToken = accessToken
    this.provider = provider
  }
}
