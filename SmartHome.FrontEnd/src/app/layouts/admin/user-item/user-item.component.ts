import { Component, Input } from '@angular/core';
import { User } from '../../../../backend/services/User/models/User';
import { AdminService } from '../../../../backend/services/Admin/admin.service';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrl: './user-item.component.css'
})
export class UserItemComponent {
  @Input() user: User | null = null;
  error:string | null = null;
  success:string | null = null; 

  constructor(private readonly _adminService: AdminService) {}
  ngOnInit(): void {
    console.log(this.user);
  }

  deleteAdmin(id: string): void {
    this._adminService.deleteAdmin(id).subscribe({
      next: (response) => {
        console.log(response);
        this.error = null;
        this.success = 'Admin deleted successfully';
      },
      error: (error) => {
        console.error(error);
        this.error = 'Error deleting admin';
        this.success = null;
      }
    });
  }

}
