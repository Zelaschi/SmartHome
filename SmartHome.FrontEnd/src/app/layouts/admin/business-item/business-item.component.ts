import { Component, Input } from '@angular/core';
import { Business } from '../../../../backend/services/Business/models/Business';

@Component({
  selector: 'app-business-item',
  templateUrl: './business-item.component.html',
  styleUrl: './business-item.component.css'
})
export class BusinessItemComponent {
  @Input() business: Business | null = null;

}
