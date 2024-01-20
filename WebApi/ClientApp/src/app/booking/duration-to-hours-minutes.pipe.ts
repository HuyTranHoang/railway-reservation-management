import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'durationToHoursMinutes'
})
export class DurationToHoursMinutesPipe implements PipeTransform {

  transform(durationInMinutes: number): string {
    const hours = Math.floor(durationInMinutes / 60);
    const minutes = durationInMinutes % 60;

    const hoursString = hours < 10 ? `0${hours}` : `${hours}`;
    const minutesString = minutes < 10 ? `0${minutes}` : `${minutes}`;

    return `${hoursString}h ${minutesString}m`;
  }

}
