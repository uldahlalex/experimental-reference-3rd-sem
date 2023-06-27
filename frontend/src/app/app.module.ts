import {APP_INITIALIZER, NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {PreloadAllModules, RouteReuseStrategy, RouterModule, Routes} from '@angular/router';
import {IonicModule, IonicRouteStrategy, NavController} from '@ionic/angular';
import {AppComponent} from './app.component';
import {TabBarComponent} from "./pages/tabbar/tab-bar.component";
import {AuthorsComponent} from "./pages/authors/authors.component";
import {BooksComponent} from "./pages/books/books.component";
import {CommonModule, NgOptimizedImage} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {SettingsModalComponent} from "./pages/books/settings/settings-modal.component";
import {AccountComponent} from "./pages/account/account.component";
import {AuthenticationService} from "../services/http/http.authentication.service";
import {BookService} from "../services/http/http.book.service";
import {SpecificBookComponent} from "./pages/books/specific-book/specific-book.component";
import {ReadinglistService} from "../services/http/http.readinglist.service";
import {BookCardComponent} from "./reusables/book-card/book-card.component";
import {ReadingListButtonComponent} from "./reusables/reading-list-button/reading-list-button.component";
import {authGuard} from "../services/auth.guard";
import {initialize} from "../services/initialize";
import {LoginComponent} from "./pages/login/login.component";


const routes: Routes = [
  {
    path: '',
    component: TabBarComponent,
    children: [
      {
        path: 'login',
        component: LoginComponent,
      },
      {
        path: 'books',
        component: BooksComponent,
        canActivate: [authGuard()]
      },
      {
        path: 'books/:id',
        component: SpecificBookComponent,
        canActivate: [authGuard()]
      },
      {
        path: 'authors',
        component: AuthorsComponent,
        canActivate: [authGuard()]
      },
      {
        path: 'account',
        component: AccountComponent,
        canActivate: [authGuard()]
      },
      {
        path: '**',
        redirectTo: 'login',
        pathMatch: 'full'
      }
    ]
  }
];


@NgModule({
  declarations: [LoginComponent, AccountComponent, TabBarComponent, SettingsModalComponent, AppComponent, BooksComponent, AuthorsComponent, SpecificBookComponent, BookCardComponent, ReadingListButtonComponent],
  imports: [
    BrowserModule,
    CommonModule,
    FormsModule,
    BrowserAnimationsModule,
    IonicModule.forRoot({
      mode: 'ios', animated: true
    }),
    RouterModule.forRoot(routes, {preloadingStrategy: PreloadAllModules}),
    NgOptimizedImage,
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: initialize,
      deps: [AuthenticationService, BookService, ReadinglistService],
      multi: true
    },
    {provide: NavController},
    {provide: RouteReuseStrategy, useClass: IonicRouteStrategy}
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }



