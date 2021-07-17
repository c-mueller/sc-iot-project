import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'formatDate'
})
export class FormatDatePipe implements PipeTransform {

  transform(isoDate: string): string {
    const d = new Date(isoDate);
    if (d.getFullYear() === 1) {
      return "Never";
    }
    return d.getDate().toString().padStart(2, '0') + "." +
      (d.getMonth() + 1).toString().padStart(2, '0') + "." +
      d.getFullYear() +
      " " +
      d.getHours().toString().padStart(2, '0') + ":" +
      d.getMinutes().toString().padStart(2, '0') + ":" +
      d.getSeconds().toString().padStart(2, '0');
  }
}
