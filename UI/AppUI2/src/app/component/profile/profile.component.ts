import { DatePipe, NgIf } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { UserInfo, UserTransaction } from '../../model/model';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css',
})
export class ProfileComponent implements OnInit {
  isProductVisible: boolean = false;
  user: UserInfo = new UserInfo();
  purchases: UserTransaction[] = [];
  adminService = inject(AdminService);

  ngOnInit(): void {
    this.getUserInfo(sessionStorage.getItem('userId') ?? '');
    this.isProductVisible = false;
  }

  getUserInfo(Id: string) {
    this.adminService.getAllUserInfoById(Id).subscribe((res: any) => {
      this.user = res;
    });
  }

  showPurchaseHistory() {
    this.isProductVisible = true;
    this.getUserPurchases(sessionStorage.getItem('userId') ?? '');
  }

  getUserPurchases(Id: string){
    this.adminService.getUserPurchases(Id).subscribe((res: any)=>{
      this.purchases = res;
    })
  }
}
