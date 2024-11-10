import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { UsersService } from '../../../../backend/services/User/users.service';
import UserStatus from './models/user.status';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  private _usersSubscription: Subscription | null = null;
  pageNumber: number = 1;
  pageSize: number = 10;
  loading: boolean = false;

  constructor(private readonly _usersService: UsersService) {}

  status: UserStatus = {
    loading: true,
    users: [],
  }

  ngOnDestroy(): void {
    this._usersSubscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this._usersService.getAllUsers(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.status ={
          users: response.data,
        }
        console.log(this.status.users);
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
    this.loadUsers();
  }
}
