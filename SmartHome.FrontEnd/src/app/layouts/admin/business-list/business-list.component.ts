import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { BusinessService } from '../../../../backend/services/Business/business.service';
import BusinessStatus from './models/business.status';
import businessFilters from './models/businessFilters';
import { FormControl, FormGroup } from '@angular/forms';

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

  filters: businessFilters = {
    businessName: null,
    fullName: null,
  }

  constructor(private readonly _businessService: BusinessService) {}

  status: BusinessStatus = {
    moreBusinesses: true,
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
    this._businessService.getBusinesses(this.pageNumber, this.pageSize, this.filters.businessName, this.filters.fullName).subscribe({
      next: (response) => {
        var moreBusinesses = response.data.length === this.pageSize;
        if (moreBusinesses){
          this.status ={
            businesses: response.data,
            moreBusinesses: true,
          }
        }
        else{
          this.status = {
            businesses: response.data,
            moreBusinesses: false,
          }
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

  readonly formField: any = {
    businessName: {
      name: 'businessName',
    },
    fullName: {
      name: 'fullName',
    },
  }

  readonly businessFilterForm = new FormGroup({
    [this.formField.businessName.name]: new FormControl('', []),
    [this.formField.fullName.name]: new FormControl('', []),
  });

  onSubmit(values : any){
    console.log('Datos del business:', values);
    this.status.loading =  true;
    this.filters = {
      businessName: values.businessName,
      fullName: values.fullName,
    }
    this.loadProducts();
  }
}
