import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { CommentAdministrationService,
  CommentDTOAdministration
} from 'src/app/services/nswag.generated.service';
import { LoggedService } from 'src/app/services/logged.service';
import * as jwt_decode from 'jwt-decode';

@Component({
  selector: 'app-administration-comment-detail',
  templateUrl: './administration-comment-detail.component.html',
  styleUrls: ['./administration-comment-detail.component.scss']
})
export class AdministrationCommentDetailComponent implements OnInit {

  comment: CommentDTOAdministration;
  isUpdating = false;

  constructor(
    private commentService: CommentAdministrationService,
    private location: Location,
    private route: ActivatedRoute,
    private authService: LoggedService
  ) { }

  ngOnInit(): void {
    this.getComment();
  }

  get isAdmin(){
    var token = this.authService.getToken();
        
    var decodedoken = jwt_decode(token);
    var currentRole = decodedoken['role'];

    if (currentRole.includes('Admin')) {
      return true;
    }
    return false;
  }

  getComment(){
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.getCommentById(id)
    .subscribe(comment => this.comment = comment)
  }

  updating(){
    this.isUpdating = true;
  }

  save(): void {
    this.commentService.updateComment(this.comment)
      .subscribe(comment => {
        this.comment = comment;
        this.isUpdating = false;
        this.goBack();
    });
  }

  delete(){
    const id = this.route.snapshot.paramMap.get('id');
    this.commentService.deleteCommentById(id)
    .subscribe(()=>this.goBack());
  }

  goBack(){
    this.location.back();
  }

}
