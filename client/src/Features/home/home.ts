import { Component, inject, Input, signal } from '@angular/core';
import { Register } from "../account/register/register";
import { AccountService } from '../../core/services/account-service';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  // @Input({required:true}) membersFromHome: User[]=[]//[] no cache therw compile time
  protected registerMode = signal(false);
  protected accountService = inject(AccountService);

  showRegister(value: boolean) {
    this.registerMode.set(value);
  }
}
