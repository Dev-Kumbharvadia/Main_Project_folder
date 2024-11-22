import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MiscService {

  constructor() { }

  setCookie(name: string, value: string, days: number): void {
    const date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    const expires = `expires=${date.toUTCString()}`;
    document.cookie = `${name}=${value}; ${expires}; path=/`;
  }
}
