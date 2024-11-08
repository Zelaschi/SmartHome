import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css',
  standalone: true,
})
export class PaginationComponent {
  @Input() pageNumber: number = 1;
  @Input() pageSize: number = 10;
  @Output() pageChange = new EventEmitter<number>();
  
  nextPage(): void {
    this.pageChange.emit(this.pageNumber + 1);
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageChange.emit(this.pageNumber - 1);
    }
  }
  
}
