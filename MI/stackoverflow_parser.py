import xml.sax
import xml.etree.ElementTree as ET


class GroupHandler(xml.sax.ContentHandler):
    fileCounter = 1
    posts = []

    def startElement(self, name, attrs):
        self.current = name
        if attrs.get("PostTypeId") == "1":

            post = {}
            post["Id"] = attrs.get("Id") if  (
                "Id" in attrs.keys()) else '0'
            )
            # post["Title"] = attrs.get("Title")
            # post["Body"] = attrs.get("Body")
            # post["Tags"] = attrs.get("Tags") if (
            #     "Tags" in attrs.keys()) else '0'
            post["Score"] = attrs.get("Score") if (
                "Score" in attrs.keys()) else '0'
            post["FavoriteCount"] = attrs.get("FavoriteCount") if (
                "FavoriteCount" in attrs.keys()) else '0'
            post["ViewCount"] = attrs.get("ViewCount") if (
                "ViewCount" in attrs.keys()) else '0'
            post["AnswerCount"] = attrs.get("AnswerCount") if (
                "AnswerCount" in attrs.keys()) else '0'
            post["CommentCount"] = attrs.get("CommentCount") if (
                "CommentCount" in attrs.keys()) else '0'

            if post["Id"] != '0'
                self.posts.append(post)

            if len(self.posts) == 1000000:
                root = ET.Element("posts")
                print(self.fileCounter)
                for currentPost in self.posts:

                    child = ET.Element("post")
                    root.append(child)
                    id = ET.SubElement(child, "Id")
                    id.text = currentPost["Id"]
                    # title = ET.SubElement(child, "Title")
                    # title.text = currentPost["Title"]
                    # body = ET.SubElement(child, "Body")
                    # body.text = currentPost["Body"]
                    # tags = ET.SubElement(child, "Tags")
                    # tagParts = currentPost["Tags"].split(">")
                    # for tagPart in tagParts:
                    #     if(tagPart != "&lt" and tagPart != ""):
                    #         tagPart = tagPart.replace("<", "")
                    #         if(tagPart in TagHandler.dictionary):
                    #             tagChild = ET.Element("Tag")
                    #             tags.append(tagChild)
                    #             tagId = ET.SubElement(tagChild, "Id")
                    #             tagId.text = TagHandler.dictionary[tagPart][0]
                    #             tagName = ET.SubElement(tagChild, "Name")
                    #             tagName.text = tagPart
                    #             tagCount = ET.SubElement(tagChild, "Count")
                    #             tagCount.text = TagHandler.dictionary[tagPart][1]
                    score = ET.SubElement(child, "Score")
                    score.text = currentPost["Score"]
                    favouriteCount = ET.SubElement(child, "FavoriteCount")
                    favouriteCount.text = currentPost["FavoriteCount"]
                    viewCount = ET.SubElement(child, "ViewCount")
                    viewCount.text = currentPost["ViewCount"]
                    answerCount = ET.SubElement(child, "AnswerCount")
                    answerCount.text = currentPost["AnswerCount"]
                    commentCount = ET.SubElement(child, "CommentCount")
                    commentCount.text = currentPost["CommentCount"]

                tree = ET.ElementTree(root)
                filename = "BigData_noText_" + str(self.fileCounter) + ".xml"
                print(filename)
                with open(filename, "wb") as fh:
                    tree.write(fh)
                self.fileCounter += 1
                self.posts.clear()

    def endElement(self, name):
        # print("Text: {}".format(self.name))
        self.current = ''


class TagHandler(xml.sax.ContentHandler):

    dictionary = {}

    def startElement(self, name, attrs):
        tag = {}
        tag["Id"] = attrs.get("Id")
        tag["TagName"] = attrs.get("TagName")
        tag["Count"] = attrs.get("Count")
        if tag["Count"] != "0":
            key = tag["TagName"]

            self.dictionary[key] = [tag["Id"], tag["Count"]]


# tagHandler = TagHandler()
# parser = xml.sax.make_parser()
# parser.setContentHandler(tagHandler)
# parser.parse('Tags.xml')

handler = GroupHandler()
parser = xml.sax.make_parser()
parser.setContentHandler(handler)
parser.parse('F:\StackOverflowdata\Posts.xml')
