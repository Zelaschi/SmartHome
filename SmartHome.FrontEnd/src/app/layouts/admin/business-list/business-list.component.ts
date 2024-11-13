import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { BusinessService } from '../../../../backend/services/Business/business.service';
import BusinessStatus from './models/business.status';

@Component({
  selector: 'app-business-list',
  templateUrl: './business-list.component.html',
  styleUrl: './business-list.component.css'
})
export class BusinessListComponent {
  private _businessSubscription: Subscription | null = null;
  pageNumber: number = 1;
  pageSize: number = 10;
  loading: boolean = false;

  constructor(private readonly _businessService: BusinessService) {}

  status: BusinessStatus = {
    loading: true,
    businesses: [],
  }

  ngOnDestroy(): void {
    this._businessSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this._businessService.getBusinesses(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.status ={
          businesses: response.data,
        }
        console.log(this.status.businesses);
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  onPageChange(page: number): void {
    this.pageNumber = page;
    this.loadProducts();
  }
}
