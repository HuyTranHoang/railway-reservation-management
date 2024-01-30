import { Pipe, PipeTransform } from '@angular/core'
import { Ticket } from '../../../core/models/bookingHistory'

@Pipe({
  name: 'ticketCodeFilter'
})

export class TicketCodeFilterPipe implements PipeTransform {

  transform(tickets: Ticket[], searchText: string): Ticket[] {
    if (!tickets || !searchText) {
      return tickets
    }

    searchText = searchText.toLowerCase()
    return tickets.filter(ticket => ticket.code.toLowerCase().includes(searchText))
  }

}
