import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../../../../backend/services/User/models/User';

@Component({
  selector: 'app-home-add-member-item',
  templateUrl: './home-add-member-item.component.html',
  styleUrl: './home-add-member-item.component.css'
})
export class HomeAddMemberItemComponent {
  @Input() user: User | null = null;
  @Output() userAdded = new EventEmitter<User>();

  addToHome(): void {
    if (this.user) {
      console.log(`Adding user ${this.user.name} to home.`);
      this.userAdded.emit(this.user);
    }
  }
}
