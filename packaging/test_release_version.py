import unittest
from release_version import version, should_release

class ReleaseVersionTests(unittest.TestCase):
    def test_version_change_on_master(self):
        self.assertTrue(should_release("push", "refs/heads/master", "2.0.1", "2.0.0"))
        self.assertFalse(should_release("push", "refs/heads/master", "2.0.1", "2.0.1"))
    def test_pull_requests_and_branches_do_not_publish(self):
        self.assertFalse(should_release("pull_request", "refs/pull/140/merge", "2.0.1", "2.0.0"))
        self.assertFalse(should_release("workflow_dispatch", "refs/heads/feature/test", "2.0.1", None))
    def test_tag_validation_and_manual_retry(self):
        self.assertTrue(should_release("push", "refs/tags/v2.0.0-preview.1", "2.0.0-preview.1", None))
        with self.assertRaises(ValueError): should_release("push", "refs/tags/v2.0.0", "2.0.1", None)
        self.assertTrue(should_release("workflow_dispatch", "refs/heads/master", "2.0.1", "2.0.1"))
    def test_explicit_version_required(self):
        self.assertEqual(version("<Project><PropertyGroup><Version>2.0.0-preview.1</Version></PropertyGroup></Project>"), "2.0.0-preview.1")
        with self.assertRaises(ValueError): version("<Project />")

if __name__ == "__main__": unittest.main()
