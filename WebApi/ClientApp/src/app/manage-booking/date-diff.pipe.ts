import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateDiff'
})

export class DateDiffPipe implements PipeTransform {
  transform(departureTime: Date): string {
    if (!departureTime) {
      return '';
    }

    const now = new Date();
    const timeDiff = departureTime.getTime() - now.getTime();

    const days = Math.floor(timeDiff / (1000 * 60 * 60 * 24));
    const remainingHours = Math.floor((timeDiff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const hoursInADay = 24;

    const daysText = days > 0 ? `${days} Days` : '';
    const hoursText = remainingHours > 0 ? `${remainingHours} Hours` : '';

    // Concatenate days and hours
    return `${daysText} ${hoursText}`.trim();
  }
}
