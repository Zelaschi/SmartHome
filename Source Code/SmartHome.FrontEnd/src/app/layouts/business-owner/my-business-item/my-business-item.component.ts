import { Component, OnInit } from '@angular/core';
import { BusinessService } from '../../../../backend/services/Business/business.service';
import BusinessCreatedModel from '../../../../backend/services/Business/models/BusinessCreatedModel';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-my-business-item',
  templateUrl: './my-business-item.component.html',
  styleUrl: './my-business-item.component.css'
})
export class MyBusinessItemComponent implements OnInit {
  business: BusinessCreatedModel | undefined;
  loading: boolean = true;
  imageUrl: SafeUrl | undefined;
  showDefaultImage: boolean = false;
  defaultImageUrl: string = 'https://qplexus.com/wp-content/uploads/2016/02/default-logo.png';

  constructor(
    private businessService: BusinessService,
    private sanitizer: DomSanitizer
  ) { }

  ngOnInit(): void {
    this.businessService.getBusinessByUser().subscribe({
      next: (business) => {
        this.business = business;
        this.processBusinessImage(business.logo);
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading business:', error);
        this.loading = false;
      }
    });
  }

  private processBusinessImage(logoUrl: string | undefined): void {
    if (!logoUrl) {
      this.showDefaultImage = true;
      return;
    }

    try {
      new URL(logoUrl);
      this.imageUrl = this.sanitizer.bypassSecurityTrustUrl(logoUrl);
      this.showDefaultImage = false;
    } catch (e) {
      console.error('Invalid URL format:', logoUrl);
      this.showDefaultImage = true;
    }
  }

  handleImageError(event: any): void {
    console.log('Image failed to load, showing default image');
    this.showDefaultImage = true;
  }
}
