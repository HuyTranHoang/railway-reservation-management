import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SharedService {

  constructor() {
  }

  sortItems(queryParams: any, sort: string, sortStates: any): { queryParams: any, sortStates: any } {
    const sortType = sort.split('Asc')[0];
    if (queryParams.sort === sort) {
      queryParams.sort = sort.endsWith('Asc') ? sort.replace('Asc', 'Desc') : sort.replace('Desc', 'Asc');
      sortStates[sortType] = !sortStates[sortType];
    } else {
      queryParams.sort = sort;
      for (const key in sortStates) {
        if (sortStates.hasOwnProperty(key)) {
          sortStates[key] = false;
        }
      }
      sortStates[sortType] = sort.endsWith('Asc');
    }

    return { queryParams, sortStates };
  }
}
