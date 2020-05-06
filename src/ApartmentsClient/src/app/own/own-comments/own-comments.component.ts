import { Component, OnInit } from '@angular/core';
import { CommentUserService,
  CommentDTO } from 'src/app/core/nswag.generated.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-own-comments',
  templateUrl: './own-comments.component.html',
  styleUrls: ['./own-comments.component.scss']
})
export class OwnCommentsComponent implements OnInit {

  ownComments: CommentDTO[];

  constructor(public router: Router, public commentService: CommentUserService) { }

  ngOnInit(): void {
    this.getOwnComments();
  }

  getOwnComments(){
    this.commentService.getAllCommentsByAuthorId()
      .subscribe(ownComments => this.ownComments = ownComments);
  }

}
