import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MiscService {

  constructor() { }

  setCookie(name: string, value: string, minutes: number): void {
    const date = new Date();
    date.setTime(date.getTime() + minutes * 60 * 1000);
    const expires = `expires=${date.toUTCString()}`;
    document.cookie = `${name}=${value}; ${expires}; path=/`;
  }
}
