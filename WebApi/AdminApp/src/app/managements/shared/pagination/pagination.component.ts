import {Component, EventEmitter, Input, Output} from '@angular/core';
import {Pagination} from '../../../@models/pagination';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss'],
})
export class PaginationComponent {

  @Input() selectOption: number;
  @Input() pagination: Pagination;
  @Output() pageChanged = new EventEmitter<number>();
  @Output() pageSizeChanged = new EventEmitter<number>();

  changePage(page: number) {
    if (page !== this.pagination.currentPage) {
      this.pagination.currentPage = page;
      this.pageChanged.emit(page);
    }
  }

  changePageSize(newSize: number) {
    if (newSize !== this.pagination.itemsPerPage) {
      this.pagination.itemsPerPage = newSize;
      this.pageSizeChanged.emit(newSize);
      this.changePage(1); // Reset to first page with new page size
    }
  }
  nextPage() {
    if (this.pagination.currentPage < this.pagination.totalPages) {
      this.changePage(this.pagination.currentPage + 1);
    }
  }

  previousPage() {
    if (this.pagination.currentPage > 1) {
      this.changePage(this.pagination.currentPage - 1);
    }
  }

  getDisplayedPages() {
    const totalPages = this.pagination.totalPages;
    const currentPage = this.pagination.currentPage;
    const range = 3; // Number of pages to display around the current page

    let start = currentPage - range;
    let end = currentPage + range;

    if (start < 1) {
      end += (1 - start);
      start = 1;
    }

    if (end > totalPages) {
      start -= (end - totalPages);
      end = totalPages;
    }

    start = Math.max(start, 1); // Ensure start is never less than 1
    end = Math.min(end, totalPages); // Ensure end is never more than total pages

    return Array.from({ length: (end - start) + 1 }, (_, i) => start + i);
  }

}
