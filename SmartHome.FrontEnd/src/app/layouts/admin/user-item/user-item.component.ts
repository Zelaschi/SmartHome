import { Component, Input } from '@angular/core';
import { User } from '../../../../backend/services/User/models/User';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.css'
})
export class UserItemComponent {
  @Input() user: User | null = null;

  ngOnInit(): void {
    console.log(this.user);
  }
}
