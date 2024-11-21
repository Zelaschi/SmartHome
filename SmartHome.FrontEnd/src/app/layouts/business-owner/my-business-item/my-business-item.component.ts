import { Component, OnInit } from '@angular/core';
import { BusinessService } from '../../../../backend/services/Business/business.service';
import BusinessCreatedModel from '../../../../backend/services/Business/models/BusinessCreatedModel';

@Component({
  selector: 'app-my-business-item',
  templateUrl: './my-business-item.component.html',
  styleUrl: './my-business-item.component.css'
})
export class MyBusinessItemComponent implements OnInit {
  business: BusinessCreatedModel | undefined;
  loading: boolean = true;

  constructor(private businessService: BusinessService) {
  }

  ngOnInit(): void {
    this.businessService.getBusinessByUser().subscribe({
      next: (business) => {
        this.business = business;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
